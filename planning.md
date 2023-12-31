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
  - Let's say my Stacks table has Id and Name, and my Flashcards table has Id, StackId (foreign key), front, and back. Is the idea basically that my data access layer should return a Flashcard DTO which has only Id, Front and Back, because it's unnecessary when I show or manage single flashcards?
    - >Yes, that's the correct interpretation. Using DTOs to "show the flashcards to the user without the Id of the stack it belongs to" means that your data access layer should return a Flashcard DTO that includes only the relevant information for displaying or managing single flashcards. In this case, the DTO should include the Id, Front, and Back properties, excluding the StackId, as it's not needed when working with individual flashcards. This approach simplifies data transfer and minimizes the data sent to the user interface, improving efficiency.
  - ~~Is the idea here to have database calls return DTOs containing a stack with all (or a subset) of its flashcards?~~
    - >~~Yes, the idea is to have database calls return DTOs containing a stack with all (or a subset of) its flashcards, excluding the stack's ID information. This simplifies the data presentation while maintaining a structured database design.~~
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
- Should pagination be used when browsing stacks and flashcards?
  - Yes.
- What if the config file is missing?
  - Give an error and exit (since there's probably no good default fallback value for a SQL Server connection string).
- Should DTO be used for study session history as well?
  - Yes.
- What if SQL Server doesn't work? E.g. can't create tables, or read, or write.
  - Give an error and exit.
- What should happen if a flashcard is moved to another stack?
  - >When a flashcard is moved to another stack, and that other stack is not deleted, the study history should remain unchanged. The numbers in the study history should only be affected if the flashcard is deleted or if the stack containing the flashcard is deleted.
  - So if I move a card from the "Japanese" stack to the "Chinese" stack, it would still show that the user studied 10 Japanese cards? They were Japanese cards at the time, but now one of then count as a Chinese card.
    - >In this scenario, if a Japanese card is moved to the "Chinese" stack, it should be reflected in the study history as studying 9 Japanese cards and 1 Chinese card. The study history should accurately represent the cards that were part of the study session, including their current stack affiliation.

## MVP Functionality

### Necessary Functionality
- Create SQL Server database with tables using foreign keys
  - CASCADE DELETE
- Configuration reading
- Console menu system & data presentation & user input
  - Stacks listing screen & stack management screen: add, delete, rename
  - Flashcard listing screen & flashcard management screen: add, delete, modify
    - Numbered lists when browsing flashcards in stacks
  - Study Session section
  - Study session history & screen (read-only)
- DTOs for both stacks with flashcards, and single flashcards, as well as study session history

### Possible Later Functionality
- Report generation system
  - For number of sessions
  - For average score
- More advanced flashcard structure
- More (meta) properties for stacks and flashcards

## User Interface
Main menu:
```text
Flashcards
======================================

1. [S]tudy Session
2. [M]anage Stacks & Flashcards
3. View Study [H]istory
0. [Q]uit

--------------------------------------
Select by pressing a number or letter.
```

Viewing stacks:
```text
Manage Stacks (page 1/3)
========================================

+-name-+-cards-+-----last studied----+
| Foo  | 120   | 2023-10-17 12:13:14 |
+------+-------+---------------------+
| Bar  | 43    | 2023-10-15 18:55:58 |
+------+-------+---------------------+
| Baz  | 67    | 2023-10-14 22:03:31 |
+------+-------+---------------------+

Select a Stack: _

----------------------------------------
Press [PgDown] to go to the next page,
or [Esc] to go back to the main menu.
```

Managing single stack:
```text
Managing Stack: Foo
======================================

1. [C]reate New Flashcards in Stack
2. [B]rowse Flashcards
3. [R]ename Stack
4. [D]elete Stack
0. Go Back to [S]tacks Overview

--------------------------------------
Select by pressing a number or letter.
```

Creating flashcard:
```text
Creating Flashcard in Stack: Foo
====================================

Enter a front side question: How do you declare a variab...

Enter a back side answer: _

------------------------------------
0 cards created.
Press [Esc] to go back to the stack.
```

Viewing flashcards:
```text
Flashcards in Foo (page 1/3)
========================================

+-no.-+-----------front-----------+-----------back-----------+
| 1   | How do you declare a v... | int myVariable;          |
+-----+---------------------------+--------------------------+
| 2   | What is the SQL statem... | SELECT * FROM Customers; |
+-----+---------------------------+--------------------------+
| 3   | In C#, what keyword is... | sealed                   |
+-----+---------------------------+--------------------------+

Select a Flashcard: _

----------------------------------------
Press [PgDown] to go to the next page,
or [Esc] to go back to the main menu.
```

Managing single flashcard:
```text
Managing Flashcard in Stack: Foo
===============================================

Front side question:
  How do you declare a variable in C# to store an integer?

Back side answer:
  int myVariable;

-----------------------------------------------
Press [E] to edit the flashcard, [D] to delete,
[M] to move it to a different stack,
or [Esc] to go back to the stack.
```

Study Session area:
```text
Study a Stack (page 1/3)
======================================

+-name-+-cards-+-----last studied----+
| Foo  | 121   | 2023-10-17 12:13:14 |
+------+-------+---------------------+
| Bar  | 43    | 2023-10-15 18:55:58 |
+------+-------+---------------------+
| Baz  | 67    | 2023-10-14 22:03:31 |
+------+-------+---------------------+

Select a Stack: _

--------------------------------------
Press [PgDown] to go to the next page,
or [Esc] to go back to the stack.
```

Studying:
```text
Studying Stack: Foo
====================================

+-------------------------------+
| How do you declare a variable |
| in C# to store an integer?    |
+-------------------------------+
|              ???              |
+-------------------------------+

Enter your answer:
  _

------------------------------------
Press [Esc] to go back to the stack.
```

Answering correctly:
```text
Studying Stack: Foo
============================================

+-------------------------------+
| How do you declare a variable |
| in C# to store an integer?    |
+-------------------------------+
|        int myVariable;        |
+-------------------------------+

You were correct!

--------------------------------------------
Press [Esc] to go back to the stack,
or any other key to go to the next question.
```

Answering incorrectly:
```text
Studying Stack: Foo
============================================

+-------------------------------+
| How do you declare a variable |
| in C# to store an integer?    |
+-------------------------------+
|        int myVariable;        |
+-------------------------------+

Your answer was incorrect. You wrote:
  float myVariable;

--------------------------------------------
Press [Esc] to go back to the stack,
or any other key to go to the next question.
```

Viewing history:
```text
Study History (page 1/43)
========================================

+-no.-+---------date--------+-stack-+-cards-+-result-+
| 1   | 2023-10-17 13:59:12 | Foo   | 3     | 3/3    |
+-----+---------------------+-------+-------+--------+
| 2   | 2023-10-17 11:19:47 | Foo   | 11    | 8/11   |
+-----+---------------------+-------+-------+--------+
| 3   | 2023-10-15 18:55:58 | Bar   | 7     | 6/7    |
+-----+---------------------+-------+-------+--------+

Select a Session: _

----------------------------------------
Press [PgDown] to go to the next page,
or [Esc] to go back to the main menu.
```

## Logic Checklist
- [x] Understand C# programming and basic syntax.
- [x] Know how to make a console menu & take user input
- [x] Know how to validate user input.
- [x] Learn about Data Transfer Objects (DTOs) and how to use them.
- [x] Know how to read an XML config file.
- [x] Understand SQL queries and basic syntax.
- [x] Know how to implement CRUD queries.
- [x] Understand database relationships, including foreign keys.
- [x] Understand CASCADE DELETE for table relationships.
- [x] Know how to connect C# applications to SQL Server and execute queries.
- [x] Be familiar with SQL Server and know how to create databases and tables.

## Data Structures

- Database
  - Stacks
    - Id (int, primary key)
    - SortName (string, unique)
    - ViewName (string)
  - Flashcards
    - Id (int, primary key)
    - StackId (int, foreign key -> Stack.Id, CASCADE DELETE)
    - Front (string)
    - Back (string)
  - History
    - Id (int, primary key)
    - StartDate (string)
  - HistoryToFlashcard
    - Id (int, primary key)
    - HistoryId (int, foreign key -> History.Id, CASCADE DELETE)
    - FlashcardId (int, foreign key -> Flashcard.Id, CASCADE DELETE)
    - WasCorrect (bool)
    - AnswerDate (string)
- DTOs
  - NewStack
    - ViewName (string)
  - StackListItem
    - Id (int)
    - ViewName (string)
    - Cards (int)
    - LastStudied (DateTime)
  - NewFlashcard
    - StackId (int)
    - Front (string)
    - Back (string)
  - ExistingFlashcard
    - Id (int)
    - Front (string)
    - Back (string)
  - HistoryListItem
    - Id (int)
    - StartDate (DateTime)
    - StackName (string)
    - CardsStudied (int)
    - CorrectAnswers (int)
  - NewHistory
    - StartDate (DateTime)
    - StackId (int)
    - Results (`List<CardStudyDTO>`)
  - CardStudyDTO
    - FlashcardId (int)
    - WasCorrect (bool)
    - AnswerDate (DateTime)
- Models
  - Stack
    - Id (int)
    - ViewName (string)
    - SortName (string, transformation of ViewName)
    - Cards (int)
  - Flashcard
    - Id (int)
    - Front (string)
    - Back (string)
    - Stack (`Stack`)
  - StudySession
    - Id (int)
    - StartDate (DateTime)
    - Stack (`Stack`)
    - Results (`List<CardStudy>`)
  - CardStudy
    - Card (Flashcard)
    - WasCorrect (bool)
    - AnswerDate (DateTime)
