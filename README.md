# PraySim
Simple predator and prey simulator created using SFML.NET

## Rules
1. Each cell has a tick counter which gets increased every frame.
2. If the cell is not empty and its tick counter is greater than the max life limit it turns into a empty cell(dies).
3. If the cell is an prey and `tick_counter % dup_interval` is 1 a new prey is created in `x+1` or `x-1` if unavailable.
4. If the cell is not empty it will randomly move to a position around it self.
5. If the position it moved to contained a pray and the cell that moved was a predator it will change the prey into a predator.
6. If the position it moved to contained a predator and the cell that moved was a pray it will change the prey into a predator.
7. Else it will move to the position.
