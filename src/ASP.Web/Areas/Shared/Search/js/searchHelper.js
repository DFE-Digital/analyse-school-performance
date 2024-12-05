// Helper functions
export const escapeRegExChars = (str) => str ? str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&") : '';

/**
 * Generates a flexible regular expression based on the input query.
 *
 * @param {string} query - The search query to convert into a regex pattern.
 * @returns {RegExp|null} A case-insensitive global regex for matching the query, or null if query is invalid.
 */
export const getRegex = (query) => {
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

export const highlightText = (text, regex) => {
    if (!text || !regex) return text || '';
    return text.replace(regex, "<strong>$&</strong>");
};
