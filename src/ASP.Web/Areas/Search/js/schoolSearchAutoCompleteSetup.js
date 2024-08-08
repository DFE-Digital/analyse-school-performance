import AutoComplete from '../../../scripts/autocomplete.js'

// Helper functions
const escapeRegExChars = (str) => str ? str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&") : '';

/**
 * Generates a regular expression for flexible text matching based on the input query.
 *
 * @param {string} query - The search query to convert into a regex pattern.
 * @returns {RegExp} A case-insensitive global regex for matching the query.
 *
 * This function:
 * 1. Handles empty queries by returning an empty regex.
 * 2. Splits the query into separate patterns (words).
 * 3. For each pattern:
 *    - Splits it into individual characters.
 *    - Escapes special regex characters.
 *    - Allows optional forward slashes between characters.
 * 4. Combines patterns with OR operator (|).
 * 5. Creates a case-insensitive global regex from the final pattern.
 *
 * The resulting regex can match variations of the query with optional
 * forward slashes between characters, useful for flexible text highlighting.
 */
const getRegex = (query) => {
    if (!query) return new RegExp('');
    const patterns = query.split(' ');
    const escapedPatterns = patterns.map(pattern => {
        const characters = pattern.split('');
        const escapeCharacters= characters.map(escapeRegExChars);
        return escapeCharacters.join("\\/?");
    });
    const regexStr = `(${escapedPatterns.join('|')})`;
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
    if (!value || !value.name) return "";
    const regex = getRegex(query);
    const suggestionTemplate = `${value.name}<br> Address:${value.address} <br>URN:${value.urn}, LAESTAB:${value.laestab}`
    return highlightText(`${suggestionTemplate}`, regex);
};

export const setSchoolHiddenField = value => {
    if (!value) return;
    const urnElement = document.getElementById("suggestionUrn");
    if (urnElement) urnElement.value = value.urn || "";
};

document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById('suggestionSearchTermContainer');
    if (container) {
        const existingInput = document.getElementById('searchTerm');
        if (existingInput) {
            existingInput.parentElement.removeChild(existingInput);
        }
        new AutoComplete('suggestionSearchTermContainer', existingInput.id, existingInput.name, nameInputTemplate, nameSuggestionTemplate, setSchoolHiddenField, 'search', 'suggestions');

        // Wait until this execution queue is finished to ensure the new input has been created.
        setTimeout(() => {
            // If after selecting a school from the autocomplete dropdown, the user starts typing in the search box again,
            // we want to reset the URN they previously selected, otherwise if they hit they search button they'll be taken
            // to the wrong school.
            const newInput = document.getElementById('searchTerm');
            newInput.addEventListener('input', e => {
                const urnElement = document.getElementById("suggestionUrn");
                if (!urnElement) return;

                urnElement.value = "";
            });
        }, 0);
    } else {
        console.error('AutoComplete container not found');
    }

    const form = document.getElementById('searchForm');
    if (form) {
        // If the user selected a school from the auto-complete dropdown, its URN should have been
        // added to the suggestionUrn hidden input. We prefer this because searching on a URN is
        // faster than searching by text.
        form.onsubmit = e => {
            const urnElement = document.getElementById("suggestionUrn");
            if (!urnElement || !urnElement.value) return;

            const input = document.getElementById('searchTerm');
            if (!input) return;

            if ('URLSearchParams' in window) {
                // URLSearchParams is supported, update/set the search input query string param to the selected URN and redirect.
                e.preventDefault();

                const url = new URL(window.location);
                url.searchParams.set(input.name, urnElement.value);
                window.location = url;
            }
            else {
                // URLSearchParams isn't supported, just update the search box value to the selected URN, this will cause the
                // search box to display the URN visibly while the form is being submitted but this will only happen on old
                // browsers.
                input.value = urnElement.value;
            }
        }
    } else {
        console.error('Search form not found');
    }
}); 