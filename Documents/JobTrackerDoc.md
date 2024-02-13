# Job Tracker Technical Design Document

## Overview

- Create an easy way to track what jobs have been applied to, next steps, and when to take those steps
- Have a calendar view that displays progress over time

## Problems This Solves

- Right now I'm making notecards which have to be manually sorted through, this would allow the data to be more visble at a glance

## Design

- Main view that allows input of new data fields
- Calendar view that shows what was done on different days, as well as displays colors if things were done on a certain day
- Each job should have the following fields
    - status (could be based on other data or manually set)
        - found
        - researched
        - applied
        - interviewing
        - accepted
        - rejected
    - date located
    - date applied
    - date reached out (array)
    - check in date
    - Cover letter complete date
    - Resume complete date
    - Contact
    - Contact method
    - URL for job listing
    - Notes


## UX/UI Needs

- Selecting a date should open a date picker or allow the date to be typed
- Jobs should show their most important status on the calendar page.


## Risks

- Ambitious

## Next Steps
This program is pretty barebones. The following features could be good next steps.

- Filtering based on Job Status, Company, and/or Job Title
- Sorting based on Job Status, Company, and/or Job Title
- Allowing areas to gain focus without having to double click
- Unique Job IDs
- Searching
- Add a clear date button to each date box
