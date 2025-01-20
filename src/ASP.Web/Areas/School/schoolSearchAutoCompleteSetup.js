import { getRegex, highlightText } from "../../Features/Search/searchHelper";
import { searchSuggestions } from "../../Features/Search/searchAutoCompleteSetup";

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

document.addEventListener('alpine:init', () => {
    Alpine.data('schoolSearchSuggestions', () => searchSuggestions(nameInputTemplate, nameSuggestionTemplate));
});