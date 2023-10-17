# Planning

## Clarifications
*Quoted answers come from [ChatGPT](https://chat.openai.com/) who plays the role of external stakeholder (i.e. customer).*


- Who's the intended user of this project?
  - >The intended users of this project are individuals interested in enhancing their learning and memory through a console-based flashcard application. This application is designed for those who want to create, organize, and study flashcards effectively.
- Any technological requirements other than it being C# and using SQL Server? Should ConsoleTableExt be used to draw all tables?
  - >Other than C# and SQL Server, there are no specific technological requirements mentioned. You can use ConsoleTableExt to draw tables if it helps, but this project's description doesn't explicitly mention it, so it's optional.
- Should the SQL Server connection string be configurable through a XML config file? Or hardcoded?
  - >The SQL Server connection string can be configurable through an XML config file for flexibility and ease of maintenance. This allows for easier changes without modifying the code directly.
- Should flashcards' sides be able to contain anything more than text?
  - >The project description doesn't specify additional content for flashcard sides. It's best to follow the given requirements and keep flashcard sides limited to text (question and answer) to meet the project's goals.
- Should users be able to create empty stacks? Empty flashcards?
  - >Users should be able to create empty stacks, but creating empty flashcards should be avoided. Flashcards should have meaningful content to serve their purpose, but empty stacks can provide a structure for future additions.
- Should users be able to create duplicate flashcards?
  - >Duplicate flashcards should be allowed, but with a mechanism to manage them effectively. Duplicate flashcards might serve different purposes or help reinforce learning, but users should be able to identify and manage them.
- There are advanced flashcard apps on the market, for example where the user can customize front and back by combining different fields of information. Is it enough to have flashcards just have a front property and a back property?
  - >The project specifies a simple structure with a front and back property for flashcards. While more advanced apps allow extensive customization, this project's focus is on simplicity. Expanding features beyond the project's scope can be considered in future iterations.
- Should there be more than one type of card? For example, the app could have "double-sided" cards which are shown not just front-to-back, but also back-to-front.
  - >The project description doesn't mention different types of cards. It's best to stick with the specified single-sided flashcards with a front and back for this project to meet its defined objectives.
- Stacks have names and flashcards, and flashcards have front and back. Should they carry any other "meta" information, such as creation/modified date or (for flashcards) a score for how well they've been learned?
  - >The project's description doesn't include additional "meta" information like creation/modified dates or learning scores for stacks and flashcards. However, these can be considered as potential enhancements beyond the project's scope to provide more functionality.
- Should stacks and flashcards be stored in exactly two tables (one each), or two or more tables?
  - >The project's requirement specifies that stacks and flashcards should be stored in exactly two tables, one for stacks and one for flashcards. This ensures a straightforward and structured database design.
- Should stack names be case sensitive? Should selection of them be case sensitive?
  - >The project's description doesn't specify case sensitivity for stack names or selection. It's advisable to implement case-insensitive handling for stack names and selection to enhance user-friendliness.
- Are flashcards allowed to belong to more than one stack?
  - >The project description doesn't mention flashcards belonging to more than one stack. It's best to follow the given requirements, where each flashcard belongs to a single stack, to meet the project's defined scope.
- Stack deletion leads to flashcard deletion. Does that imply that CASCADE DELETE should be used?
  - >Yes, CASCADE DELETE should be used for the relationship between stacks and flashcards in the database. This ensures that when a stack is deleted, its associated flashcards are also automatically removed to maintain data integrity.
  - ~~What of flashcards which are used in multiple stacks?~~
- Explain "DTOs (Data Transfer Objects) are used to show the flashcards to the user without the Id of the stack it belongs to".
  - >DTOs are used to provide a simplified representation of flashcards to users. They exclude the stack's ID information, making it easier to display flashcards without exposing the underlying database structure, enhancing user privacy and abstraction.
  - Is the idea here to have database calls return DTOs containing a stack with all (or a subset) of its flashcards?
    - >Yes, the idea is to have database calls return DTOs containing a stack with all (or a subset of) its flashcards, excluding the stack's ID information. This simplifies the data presentation while maintaining a structured database design.
  - Does "Id" here imply that the stacks table should use an integer primary key? Since stacks have unique names, that could also be used as primary key.
    - >The use of an "Id" implies the stacks table should have an integer primary key. While unique stack names could potentially be used as a primary key, having a separate integer ID can provide more flexibility and is a common practice in database design.
- Flashcards are shown "numbered from 1 through n". Does that imply that the flashcards table should use an integer primary key? (They could use a composite key of stack name, front side, back side.)
  - >The numbering of flashcards from 1 through n doesn't imply the primary key choice. Flashcards could use an integer primary key, but it's unrelated to the numbering requirement. The primary key may be separate, like a composite key.
- As for the Study Session section of the app, should some kind of learning strategy be implemented such as spaced repetition? Or just show random flashcards disregarding how often they've been correct or how often they've been shown?
  - >The project's description doesn't specify a learning strategy. For this project, it's sufficient to show flashcards without a sophisticated learning strategy. Implementing spaced repetition would be a significant extension beyond the project's scope.
- Should study session history store only overall score (like "8 out of 9 correct"), or store results on a per-flashcard level? Should it store anything else, like date started and time spent (per stack and/or per flashcard)?
  - >The study session history should store overall scores and individual flashcard results, allowing users to track performance. Optionally, it can store the date started and time spent per session for more detailed analysis. This provides users with comprehensive feedback on their learning progress.
- Stack deletion leads to study session history deletion. Does that imply that CASCADE DELETE should be used?
  - >Yes, CASCADE DELETE should be used for the relationship between stack deletion and the associated study session history in the database. This ensures that when a stack is deleted, its linked session history records are also removed to maintain data consistency.
- How should the question-and-answer process work? Should the user input be taken directly and compared with the answer? If so, case-insensitive with trimming of whitespace?
  - >The user input should be compared with the answer directly, with case-insensitive matching and trimming of whitespace for a more forgiving user experience. This approach ensures that minor variations in input are accepted.

### Follow-up Questions
- Since stack deletion leads to study session history deletion through CASCADE DELETE, and since individual flashcard results are to be recorded, does this mean that flashcard deletion also lead to CASCADE DELETE for study session history?
  - >Based on the project requirements, flashcard deletion should also lead to CASCADE DELETE for study session history. This ensures consistency, as flashcard results are tied to specific flashcards, and deleting them should remove associated session history to maintain data integrity.
  - How should study session overall scores function in the event that flashcards were deleted? For example, if I answered 4/5 correctly by answering correctly for flashcards with ids 1, 2, 3 and 4, and then I delete flashcard 1, should the overall score be recalculated (to 3/4) or kept intact?
    - >In the event of flashcard deletion, the overall score for a study session should be recalculated to reflect the remaining flashcards. In your example, if you initially answered 4/5 correctly and then deleted flashcard 1, the score should be recalculated to 3/4 to accurately represent your performance.
