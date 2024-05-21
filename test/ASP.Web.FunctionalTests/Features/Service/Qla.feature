Feature: Qla
@Javascript:disabled
Scenario: Qla page should be accessible when valid urn is provided
     Given establishment "136028" exists:
		"""
		{
            
            "id": "136028",
            "name": "Dagenham Park CofE School",
            "urn": "136028",
        }
		"""
    When I navigate to /school/136028/qla
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Question level analysis"
    And the element "*[id='header-navigation-link-my-school']" should have the text content "My school"
    And the element "*[data-testid='my-school-navigation']" should have the class "govuk-header__navigation-item app-header__navigation-item app-header__navigation-item--current"
    And the element "*[data-testid='qla-navigation']" should have the text content "Question level analysis"
    And the element "*[data-testid='qla-navigation']" should have the attribute "aria-current" set to "page"