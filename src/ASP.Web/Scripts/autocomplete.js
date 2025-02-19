import accessibleAutocomplete from 'accessible-autocomplete';

export default class AutoComplete {
    // Regular properties with underscore prefix for "internal" use
    _currentRequestController = null;
    _observer = null;
    _debounceTimers = new Map();
    _lastConfirmedSearchTerm = '';
    _previousResults = [];

    constructor({
        containerId,
        targetInputElementId,
        targetInputElementName,
        inputTemplate,
        suggestionTemplate,
        setHiddenField,
        queryParameter,
        searchRegenerateDelay = 250,
        minLength = 2,
        authCheckEndpoint = '/account/auth/status/',
        loginRedirectUrl = '/account/login'
    }) {
        // Validate required parameters
        this.validateConstructorParams(arguments[0]);

        // Initialize properties
        this.config = {
            containerId,
            targetInputElementId,
            targetInputElementName,
            inputTemplate,
            suggestionTemplate,
            setHiddenField,
            queryParameter,
            searchRegenerateDelay,
            minLength,
            authCheckEndpoint,
            loginRedirectUrl
        };

        this.init();
    }

    validateConstructorParams(params) {
        const requiredParams = [
            'containerId',
            'targetInputElementId',
            'targetInputElementName',
            'queryParameter'
        ];

        for (const param of requiredParams) {
            if (!params[param]) {
                throw new Error(`Missing required parameter: ${param}`);
            }
        }
    }

    hideMenu = (container) => {
        try {
            const menu = container.querySelector('[role="listbox"]');
            if (menu) {
                menu.innerHTML = '';
                menu.classList.add('autocomplete__menu--hidden');
            }
        } catch (error) {
            console.error('Error hiding menu:', error);
        }
    };

    debounce(fn, delay) {
        return (...args) => {
            const key = fn.toString();
            if (this._debounceTimers.has(key)) {
                clearTimeout(this._debounceTimers.get(key));
            }

            const timer = setTimeout(() => {
                this._debounceTimers.delete(key);
                fn(...args);
            }, delay);

            this._debounceTimers.set(key, timer);
        };
    }

    async checkAuthStatus() {
        try {
            const response = await fetch(this.config.authCheckEndpoint);
            return response.ok;
        } catch (error) {
            console.error('Authentication check failed:', error);
            return false;
        }
    }

    search = async (query, populateResults, suggestUrl) => {
        try {
            // Authentication check
            const isAuthenticated = await this.checkAuthStatus();
            if (!isAuthenticated) {
                const returnUrl = encodeURIComponent(window.location.pathname);
                window.location.href = `${this.config.loginRedirectUrl}?returnUrl=${returnUrl}`;
                return;
            }

            // Abort previous request if exists
            if (this._currentRequestController) {
                this._currentRequestController.abort();
            }

            // Create new abort controller
            this._currentRequestController = new AbortController();

            const urlWithQuery = this.buildUrl(suggestUrl, this.config.queryParameter, query);

            if (this._previousResults.length > 0) {
                populateResults(this._previousResults);
            }

            const response = await this.executeSearch(urlWithQuery);
            await this.handleSearchResponse(response, populateResults, query);

        } catch (error) {
            this.handleSearchError(error, populateResults);
        }
    };

    // Helper function to combine URL and query parameters
    buildUrl(baseUrl, queryParam, query) {
        // Split URL into path and query parts
        const [path, existingQuery] = baseUrl.split('?');

        // Create URLSearchParams from existing query (if any)
        const searchParams = new URLSearchParams(existingQuery || '');

        // Get the current value and decode it if it exists
        const currentValue = searchParams.get(queryParam);
        if (currentValue) {
            searchParams.set(queryParam, decodeURIComponent(currentValue));
        }

        // Set the new query value
        searchParams.set(queryParam, query);

        return `${path}?${searchParams.toString()}`;
    };

    async executeSearch(url) {
        return fetch(url, {
            method: "GET",
            headers: new Headers({ "Content-Type": "application/json" }),
            signal: this._currentRequestController.signal
        });
    }

    async handleSearchResponse(response, populateResults, searchTerm) {
        if (!response.ok && response.status !== 404) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }

        const data = await response.json();

        // Ensure the results are only displayed if the search term matches the latest input
        const input = document.querySelector(`#${this.config.targetInputElementId}`);
        if (input && input.value === searchTerm) {
            if (data?.length > 0) {
                // Update both the displayed results and stored previous results
                this._previousResults = data;
                this._lastConfirmedSearchTerm = searchTerm;
                populateResults(this._previousResults);
            } else {
                // If no results, clear previous results and hide the dropdown
                this._previousResults = [];
                this._lastConfirmedSearchTerm = '';
                this.hideMenu(document.querySelector(`#${this.config.containerId}`));
            }
        }
    }

    handleSearchError(error, populateResults) {
        if (error.name === 'AbortError') {
            // Keep showing previous results on abort
            if (this._previousResults.length > 0) {
                populateResults(this._previousResults);
                return;
            }
        }
        this._previousResults = [];
        this._lastConfirmedSearchTerm = '';
        populateResults([]);
    }

    setupMutationObserver(container, input) {
        this._observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                if (mutation.type === 'childList') {
                    const menu = container.querySelector('[role="listbox"]');
                    if (menu && (!input.value || input.value.length < this.config.minLength)) {
                        this.hideMenu(container);
                    }
                }
            });
        });

        const menu = container.querySelector('[role="listbox"]');
        if (menu) {
            this._observer.observe(menu, { childList: true, subtree: true });
        }
    }

    bindAutoSuggest() {
        const container = document.querySelector(`#${this.config.containerId}`);
        if (!container) {
            throw new Error(`Container with ID '${this.config.containerId}' not found`);
        }

        const suggestUrl = encodeURI(container.dataset.suggestUrl);

        accessibleAutocomplete({
            element: container,
            id: this.config.targetInputElementId,
            name: this.config.targetInputElementName,
            defaultValue: '',
            minLength: this.config.minLength,
            source: this.debounce(
                (query, populateResults) => this.search(query, populateResults, suggestUrl),
                this.config.searchRegenerateDelay
            ),
            templates: {
                inputValue: this.config.inputTemplate,
                suggestion: value => {
                    // Use _lastConfirmedSearchTerm for highlighting instead of current input value
                    return this.config.suggestionTemplate(value, this._lastConfirmedSearchTerm);
                }
            },
            onConfirm: this.config.setHiddenField,
            displayMenu: 'overlay',
            showNoOptionsFound: false
        });

        this.setupEventListeners(container);
    }

    setupEventListeners(container) {
        const input = container.querySelector(`#${this.config.targetInputElementId}`);
        if (input) {
            input.addEventListener('keydown', (event) => {
                if (event.key === 'Backspace' || event.key === 'Delete') {
                    const currentLength = event.target.value.length;
                    if (currentLength < this.config.minLength) {
                        this.hideMenu(container);
                    }
                }
            });

            this.setupMutationObserver(container, input);
        }
    }

    cleanup() {
        if (this._observer) {
            this._observer.disconnect();
        }

        // Clear all debounce timers
        for (const timer of this._debounceTimers.values()) {
            clearTimeout(timer);
        }
        this._debounceTimers.clear();

        // Abort any pending requests
        if (this._currentRequestController) {
            this._currentRequestController.abort();
        }

        this._lastConfirmedSearchTerm = '';
        this._previousResults = [];
    }

    init() {
        try {
            this.bindAutoSuggest();
        } catch (error) {
            console.error('Initialization failed:', error);
            throw error;
        }
    }
}