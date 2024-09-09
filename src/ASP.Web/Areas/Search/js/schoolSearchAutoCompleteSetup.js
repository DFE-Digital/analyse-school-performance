import AutoComplete from '../../../scripts/autocomplete.js'

// Helper functions
const escapeRegExChars = (str) => str ? str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&") : '';

/**
 * Generates a flexible regular expression based on the input query.
 *
 * @param {string} query - The search query to convert into a regex pattern.
 * @returns {RegExp|null} A case-insensitive global regex for matching the query, or null if query is invalid.
 */
const getRegex = (query) => {
    // Return null if query is empty or less than 2 characters
    if (!query || query.length < 2) return null;

    // Create a flexible query pattern from the input query string
    const flexibleQuery = query.split('').map(char => {
        // Escape special regex characters in the current character
        const escapedChar = escapeRegExChars(char);

        if (/\d/.test(char)) {
            // For digits:
            // - Keep the digit as is
            // - Allow an optional forward slash after the digit
            // This handles cases like LAESTAB numbers (e.g., "123" can match "1/2/3")
            return `${escapedChar}\\/?`;
        } else {
            // For non-digits:
            // - Keep the character as is
            // - Allow an optional space or tab after the character
            // - Allow an optional forward slash after the space/tab or directly after the character
            // This provides flexibility in matching (e.g., "ab" can match "a b", "a/b", "a /b", etc.)
            return `${escapedChar}[ \\t]?\\/?`;
        }
    }).join('');

    // Wrap the flexible query in parentheses to create a capturing group
    const regexStr = `(${flexibleQuery})`;

    // Create and return a RegExp object:
    // - 'i' flag for case-insensitive matching
    // - 'g' flag for global matching (find all occurrences)
    return new RegExp(regexStr, 'ig');
};

const highlightText = (text, regex) => {
    if (!text || !regex) return text || '';
    return text.replace(regex, "<strong>$&</strong>");
};

// Template functions
export const nameInputTemplate = value => {
    if (!value || !value.name) return "";
    return value && value.name ? `${value.name}` : "";
};

export const nameSuggestionTemplate = (value, query) => {
    if (!value || !value.name || !query || query.length < 2) return "";
    const regex = getRegex(query);
    if (!regex) return "";

    const name = highlightText(value.name, regex);
    const address = highlightText(value.address, regex);
    const urn = highlightText(value.urn, regex);
    const laestab = highlightText(value.laestab, regex);

    return `${name}<br> Address:${address} <br>URN:${urn}, LAESTAB:${laestab}`;
};

const searchSuggestions = () => ({
    suggestionUrn: '',

    init() {
        this.$nextTick(() => {
            this.initializeAutocomplete();
        });
    },

    initializeAutocomplete() {
        const container = document.getElementById('suggestionSearchTermContainer');
        if (container) {
            const existingInput = document.getElementById('searchTerm');
            if (existingInput) {
                existingInput.parentElement.removeChild(existingInput);
            }
            new AutoComplete('suggestionSearchTermContainer', existingInput.id, existingInput.name, nameInputTemplate, nameSuggestionTemplate, this.setSchoolHiddenField.bind(this), 'search', 'suggestions');

            setTimeout(() => {
                const newInput = document.getElementById('searchTerm');
                newInput.addEventListener('input', () => {
                    this.suggestionUrn = '';
                });
            }, 0);
        }
    },
    
    setSchoolHiddenField(value) {
        if (!value) return;
        this.suggestionUrn = value.urn || '';
    },

    handleSubmit(e) {
        if (!this.suggestionUrn) return;

        const input = document.getElementById('searchTerm');
        if (!input) return;

        if ('URLSearchParams' in window) {
            e.preventDefault();
            const url = new URL(window.location);
            url.searchParams.set(input.name, this.suggestionUrn);
            window.location = url;
        } else {
            input.value = this.suggestionUrn;
        }
    }
});


document.addEventListener('alpine:init', () => {
    Alpine.data('searchSuggestions', searchSuggestions);
});


