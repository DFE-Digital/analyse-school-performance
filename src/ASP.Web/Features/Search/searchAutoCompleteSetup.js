import AutoComplete from "../../scripts/autocomplete";

export const searchSuggestions = (inputTemplate, suggestionTemplate) => ({
    suggestion: '',
    autoCompleteInstance: null, // Store the AutoComplete instance for cleanup

    init() {
        this.$nextTick(() => {
            this.initializeAutocomplete();
        });
    },

    initializeAutocomplete() {
        const container = document.getElementById('suggestionSearchTermContainer');
        if (container) {
            const existingInput = document.getElementById('app-field-Search');
            if (existingInput) {
                existingInput.parentElement.removeChild(existingInput);
            }

            // Create a new AutoComplete instance and store it
            this.autoCompleteInstance = new AutoComplete({
                containerId: 'suggestionSearchTermContainer',
                targetInputElementId: existingInput.id,
                targetInputElementName: existingInput.name,
                inputTemplate: inputTemplate,
                suggestionTemplate: suggestionTemplate,
                setHiddenField: this.setHiddenField.bind(this),
                queryParameter: 'search',
                resultDataProperty: 'suggestions'
            });

            // Wait until this execution queue is finished to ensure the new input has been created.
            setTimeout(() => {
                // If the user selects a School/LA from the autocomplete dropdown and then starts typing in the search box again,
                // we want to reset the previously selected suggestion (URN/code). Otherwise, if they click the search button, 
                // they may be directed to the wrong School/LA.
                const newInput = document.getElementById('app-field-Search');
                newInput.addEventListener('input', () => {
                    this.suggestion = '';
                });
            }, 0);
        }
    },

    setHiddenField(value) {
        if (!value) return;
        this.suggestion = value.urn || value.code || '';
    },

    handleSubmit(e) {
        // If the user selects a School/LA from the autocomplete dropdown, its URN/Code should be added to the hidden suggestion input.
        // We prefer this approach because searching by URN/Code is faster than searching by text.

        if (!this.suggestion) return;

        const input = document.getElementById('app-field-Search');
        if (!input) return;

        if ('URLSearchParams' in window) {
            // URLSearchParams is supported, update/set the search input query string param to the selected suggestion and redirect.
            e.preventDefault();
            const url = new URL(window.location);
            url.searchParams.set(input.name, this.suggestion);
            window.location = url;
        } else {
            // URLSearchParams isn't supported, just update the search box value to the selected suggestion, this will cause the
            // search box to display the suggestion visibly while the form is being submitted but this will only happen on old
            // browsers.
            input.value = this.suggestion;
        }
    },

    destroyed() {
        // Call the cleanup method when the component is destroyed
        if (this.autoCompleteInstance) {
            this.autoCompleteInstance.cleanup();
            this.autoCompleteInstance = null; // Clear the reference
        }
    }
});