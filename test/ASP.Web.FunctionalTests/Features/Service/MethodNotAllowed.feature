Feature: Method Not Allowed Error

    Background:
        Given I am a logged-in user

    @Javascript:disabled
    Scenario: Application displays a method not allowed error page
        When I navigate to /error-test/method-not-allowed-error/
        Then I should get a 405 response
        And the page title should be "Method not allowed"
        And the element "*[data-testid='method-not-allowed-message']" should have the text content "The action you tried to perform is not allowed."
        And the element "*[data-testid='method-not-allowed-reason-intro']" should have the text content "This might be because:"
        And the element "*[data-testid='method-not-allowed-reason-method']" should have the text content "The method (e.g., GET, POST) used to access this page is not supported."
        And the element "*[data-testid='method-not-allowed-reason-access']" should have the text content "You are trying to access a resource in a way that is not permitted."
        And the element "*[data-testid='method-not-allowed-actions-intro']" should have the text content "You can:"
        And the element "*[data-testid='method-not-allowed-action-back']" should have the text content "Go back and try a different action."
        And the element "*[data-testid='method-not-allowed-action-contact']" should have the text content "Contact support if you believe this is an error."