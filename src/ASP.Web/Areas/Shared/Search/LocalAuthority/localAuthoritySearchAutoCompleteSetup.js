import { getRegex, highlightText } from "../js/searchHelper";
import { searchSuggestions }  from "../js/searchAutoCompleteSetup";

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
    const code = highlightText(value.code, regex);

    return `${name}<br> Code:${code}`;
};

document.addEventListener('alpine:init', () => {
    Alpine.data('laSearchSuggestions', () => searchSuggestions(nameInputTemplate, nameSuggestionTemplate));
});