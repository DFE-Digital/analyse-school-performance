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
    const container = document.getElementById('suggestionSearchTerm');
    if (container) {
        new AutoComplete('suggestionSearchTerm', nameInputTemplate, nameSuggestionTemplate, setSchoolHiddenField,
            'suggestionSearchTerm', 'suggestions');
    } else {
        console.error('AutoComplete container not found');
    }
}); 