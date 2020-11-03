# BowlingConsoleApp
10pin Bowling Console App which uses BowlingAPI


This is a console application which allows user to enter the scores for each frame and each throw.
It constructs the request and calls the API to insert/get the scores.

When you start the game:
1) need to select if you want to create a new player or play with existing player. 
    Enter 'n' for new player and any other value to continue with existing player.
         if new player is selected, we need to enter name of the player and displays the inserted player with id. from this we need to select player id
         if existing player is selected, we need to enter the name and it displays all the matching names and we can select the id of the player from the list.
2) we need enter the selected player id from above to start the game.
3) next the screen prompts the player to enter the scores for the frames 1-10 and 2 throws each, considering the strike
4) once all the 10 frames scores is entered the final score is shown. Also after each frame, the score till that frame will be shown.

How to run console app:
1) First make sure the API solution is running.
2) then we need to run the console app.

