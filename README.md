# Flashcards
An implementation of [the fifth project of The C# Academy](https://www.thecsharpacademy.com/project/14).

## Project Description
The project is a console based "flashcard" learning application: you create flashcards, where each flashcard contains a "front" with a question and a "back" with an answer; the front side is shown to test your knowledge/memory, and your given answer is tested against the back side. Flashcards are collected (by subject or other user defined criteria) in "stacks".

### Requirements
- Data is stored using SQL Server.
- Users can create flashcards and stacks.
- Stacks and flashcards are stored in two different tables, linked by a foreign key.
- Stacks have unique names.
- Every flashcard belongs to a stack.
- If a stack is deleted, all of its flashcards are deleted.
- DTOs (Data Transfer Objects) are used to "show the flashcards to the user without the Id of the stack it belongs to".
- Flashcards when shown in stacks are always numbered from 1 through n without gaps.
- The application has a "Study Session" where the user can study their stacks.
- Study session history is stored with date and score.
- If a stack is deleted, all of its study session history is deleted, through table linking.
- Users can browse their study session history, but not modify it (other than through deleting stacks).
- Users can select stack by entering its name.

### Bonus Features
- [ ] A report system can show the **number of sessions** per month per stack for a given year, where data is gotten by pivoting database tables.
- [ ] A report system can show the **average score** per month per stack for a given year, where data is gotten by pivoting database tables.
