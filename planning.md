# Planning

## Clarifications
*Quoted answers come from [ChatGPT](https://chat.openai.com/) who plays the role of external stakeholder (i.e. customer).*


- Who's the intended user of this project?
- Any technological requirements other than it being C# and using SQL Server? Should ConsoleTableExt be used to draw all tables?
- Should the SQL Server connection string be configurable through a XML config file? Or hardcoded?
- Should flashcards' sides be able to contain anything more than text?
- Should users be able to create empty stacks? Empty flashcards?
- Should users be able to create duplicate flashcards?
- There are advanced flashcard apps on the market, for example where the user can customize front and back by combining different fields of information. Is it enough to have flashcards just have a front property and a back property?
- Should there be more than one type of card? For example, the app could have "double-sided" cards which are shown not just front-to-back, but also back-to-front.
- Stacks have names and flashcards, and flashcards have front and back. Should they carry any other "meta" information, such as creation/modified date or (for flashcards) a score for how well they've been learned?
- Should stacks and flashcards be stored in exactly two tables (one each), or two or more tables?
- Should stack names be case sensitive? Should selection of them be case sensitive?
- Are flashcards allowed to belong to more than one stack?
- Stack deletion leads to flashcard deletion. Does that imply that CASCADE DELETE should be used?
  - What of flashcards which are used in multiple stacks?
- Explain "DTOs (Data Transfer Objects) are used to show the flashcards to the user without the Id of the stack it belongs to".
  - Is the idea here to have database calls return DTOs containing a stack with all (or a subset) of its flashcards?
  - Does "Id" here imply that the stacks table should use an integer primary key? Since stacks have unique names, that could also be used as primary key.
- Flashcards are shown "numbered from 1 through n". Does that imply that the flashcards table should use an integer primary key? (They could use a composite key of stack name, front side, back side.)
- As for the Study Session section of the app, should some kind of learning strategy be implemented such as spaced repetition? Or just show random flashcards disregarding how often they've been correct or how often they've been shown?
- Should study session history store only overall score (like "8 out of 9 correct"), or store results on a per-flashcard level? Should it store anything else, like date started and time spent (per stack and/or per flashcard)?
- Stack deletion leads to study session history deletion. Does that imply that CASCADE DELETE should be used?
- How should the question-and-answer process work? Should the user input be taken directly and compared with the answer? If so, case-insensitive with trimming of whitespace?
