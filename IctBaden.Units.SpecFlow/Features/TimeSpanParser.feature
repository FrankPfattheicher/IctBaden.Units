@UnitTests
Feature: TimeSpanParser
Parse second based TimeSpans given in natural text format
i.e.:
"26" 		which means seconds
"1:22" 		minutes and seconds
"2:30:50" 	hours, minutes and seconds
"3.2:30:50" days, hours, minutes and seconds

    Scenario: Parse invalid input
        Given 'Text' is invalid
        When parsed 'Text' as 'TimeSpan'    
        Then the 'TimeSpan' should be 0 total seconds

    Scenario: Parse simple number
        laber bla
        Given 'Text' is <text>
        When parsed 'Text' as 'TimeSpan'
        Then the 'TimeSpan' should be <value> total seconds
        Examples: 
          |  text |  value |
          |   26  |   26   |
          |   01  |    1   |
          |   -2  |   -2   |
          |  120  |  120   |

    Scenario: Parse minutes and seconds
        Given 'Text' is 1:22
        When parsed 'Text' as 'TimeSpan'
        Then the 'TimeSpan' should be 82 total seconds
        
    Scenario: Parse negative minutes and seconds
        Given 'Text' is -1:22
        When parsed 'Text' as 'TimeSpan'
        Then the 'TimeSpan' should be -82 total seconds
        
    Scenario: Parse hours, minutes and seconds
        Given 'Text' is 2:30:23
        When parsed 'Text' as 'TimeSpan'
        Then the 'TimeSpan' should be 9023 total seconds

    Scenario: Parse days, hours, minutes and seconds
        Given 'Text' is 7.08:45:59
        When parsed 'Text' as 'TimeSpan'
        Then the 'TimeSpan' should be 7 days, 8 hours, 45 minutes, 59 seconds

        