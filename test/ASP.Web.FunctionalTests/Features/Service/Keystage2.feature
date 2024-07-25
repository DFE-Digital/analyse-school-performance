Feature: Keystage2
@Javascript:disabled
Scenario: Key stage 2 page should be accessible when valid urn is provided
    Given establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    When I navigate to /school/136028/key-stage-2
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Key stage 2"
    And the element "*[id='header-navigation-link-my-school']" should have the text content "My school"
    And the element "*[data-testid='my-school-navigation']" should have the class "govuk-header__navigation-item app-header__navigation-item app-header__navigation-item--current"
    And the element "*[data-testid='key-stage-2-navigation']" should have the text content "Key stage 2"
    And the element "*[data-testid='key-stage-2-navigation']" should have the attribute "aria-current" set to "page"