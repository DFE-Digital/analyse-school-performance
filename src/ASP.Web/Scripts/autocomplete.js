import accessibleAutocomplete from 'accessible-autocomplete'

export default class AutoComplete {
    constructor(containerId, targetInputElementId, targetInputElementName, inputTemplate, suggestionTemplate, setHiddenField,
                queryParameter, resultDataProperty, searchRegenerateDelay = 500, minLength = 2) {
        this.containerId = containerId;
        this.targetInputElementId = targetInputElementId;
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

    async checkAuthStatus() {
        try {
            const response = await fetch('/account/auth/status/');
            return response.ok;
        } catch (error) {
            console.error('Error checking auth status:', error);
            return false;
        }
    }

    search = async (query, populateResults, suggestUrl) => {
        console.log(`Search triggered with query: ${query}`);

        const isAuthenticated = await this.checkAuthStatus();
        if (!isAuthenticated) {
            console.log('Not authenticated. Redirecting to login...');
            window.location.href = '/account/login?returnUrl=' + encodeURIComponent(window.location.pathname);
            return;
        }

        const encodedQuery = encodeURIComponent(query);
        const urlWithQuery = `${suggestUrl}?${this.queryParameter}=${encodedQuery}`;

        if (this.currentRequestSignal) {
            this.currentRequestSignal.abort();
        }
        const controller = new AbortController();
        this.currentRequestSignal = controller;

        try {
            const response = await fetch(urlWithQuery, {
                method: "GET",
                headers: new Headers({"Content-Type": "application/json"}),
                signal: controller.signal
            });

            if (!response.ok) {
                console.error(`Network response was not ok: ${response.status} ${response.statusText}`);
                populateResults([]);
                return;
            }

            const data = await response.json();
            if (data && data[this.resultDataProperty]) {
                populateResults(data[this.resultDataProperty]);
            } else {
                console.log('No results found or invalid data structure');
                populateResults([]);
            }
        } catch (error) {
            if (error.name === 'AbortError') {
                console.log('Request was aborted');
            } else {
                console.error('Error fetching search data:', error);
            }
            populateResults([]);
        }
    }
    bindAutoSuggest() {
        const container = document.querySelector(`#${this.containerId}`);
        const suggestUrl = encodeURI(container.dataset.suggestUrl);

        accessibleAutocomplete({
            element: container,
            id: this.targetInputElementId,
            name: this.targetInputElementName,
            minLength: this.minLength,
            source: this.debounce((query, populateResults) => this.search(query, populateResults, suggestUrl), this.searchRegenerateDelay),
            templates: {
                inputValue: this.inputTemplate,
                suggestion: value => this.suggestionTemplate(value, document.querySelector(`#${this.containerId} #${this.targetInputElementId}`).value)
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
