import accessibleAutocomplete from 'accessible-autocomplete'

export default class AutoComplete {
    constructor(targetInputElementName, inputTemplate, suggestionTemplate, setHiddenField,
                queryParameter, resultDataProperty, searchRegenerateDelay = 500, minLength = 2) {
        this.targetInputElementName = targetInputElementName;
        this.inputTemplate = inputTemplate;
        this.suggestionTemplate = suggestionTemplate;
        this.setHiddenField = setHiddenField;
        this.minLength = minLength;
        this.queryParameter = queryParameter;
        this.resultDataProperty = resultDataProperty;
        this.searchRegenerateDelay = searchRegenerateDelay;
        this.init();
    }

    debounce(fn, delay = 500) {
        let timer;
        return (...args) => {
            clearTimeout(timer);
            timer = setTimeout(() => fn(...args), delay);
        };
    }

    search = (query, populateResults, suggestUrl) => {
        console.log(`Search triggered with query: ${query}`);  // Check if search is triggered
        const encodedQuery = encodeURIComponent(query);
        const urlWithQuery = `${suggestUrl}?${this.queryParameter}=${encodedQuery}`;

        if (this.currentRequestSignal) {
            this.currentRequestSignal.abort();
        }
        const controller = new AbortController();
        this.currentRequestSignal = controller;

        fetch(urlWithQuery, {
            method: "GET",
            headers: new Headers({"Content-Type": "application/json"}),
            signal: controller.signal
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                populateResults(data[this.resultDataProperty]);
            })
            .catch(error => {
                if (error.name !== 'AbortError') {
                    console.error('Error fetching search data:', error);
                }
            });
    }
    
    bindAutoSuggest() {
        const inputElement = document.querySelector(`#${this.targetInputElementName}`);
        const suggestUrl = encodeURI(inputElement.dataset.suggestUrl);

        accessibleAutocomplete({
            element: inputElement,
            id: this.targetInputElementName,
            name: this.targetInputElementName,
            minLength: this.minLength,
            source: this.debounce((query, populateResults) => this.search(query, populateResults, suggestUrl), this.searchRegenerateDelay),
            templates: {
                inputValue: this.inputTemplate,
                suggestion: value => this.suggestionTemplate(value, document.querySelector(`#${this.targetInputElementName} #${this.targetInputElementName}`).value)
            },
            onConfirm: value => this.setHiddenField(value),
            displayMenu: 'overlay',
            showNoOptionsFound: false
        });
    }

    init() {
        this.bindAutoSuggest();
    }

}
