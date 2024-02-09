# Tool Request Template

## Overview


Summary or goal of the thing you want. Example: Make it so texture imports are handled automatically.

- Create an easy way to track what jobs have been applied to, next steps, and when to take those steps
- Have a calendar view that displays progress over time

## Problems This Solves


A list of problems that are currently pressing and how this solves them. Example: environment art - we are fixing the pipeline from maya to unity to have x less steps.

- Right now I'm making notecards which have to be manually sorted through, this would allow the data to be more visble at a glance

## Design


Up to one page describing what this is and what it needs. The first version does not need to account for every case, it's expected this doc will go back and forth at least once. Ex: Currently settings X, Y, and Z have to be set manually with each import. This would automatically set those when N conditions are met. Include a "why" for each field.

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


If you need assistance in letting the player/user know what to do when... put what you need them to do when. Ex: It would be nice if all that needed to be done was dragging and dropping into the folder and things happened automagically. (mockups can go here)

- Selecting a date should open a date picker or allow the date to be typed
- Jobs should show their most important status on the calendar page.


## Risks


If you see any deadline/feature/risks with how this might integrate with another system etc. Ex: This may break the way that previous textures were imported so it would be good to check for that.

- Ambitious