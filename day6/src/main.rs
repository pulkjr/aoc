use std::{fs, usize};

fn get_position(lines: &Vec<Vec<char>>, c: &char) -> Option<Vec<(usize, usize)>> {
    let mut elements: Vec<(usize, usize)> = Vec::new();
    for (outer_i, outer_vec) in lines.iter().enumerate() {
        for (inner_i, element) in outer_vec.iter().enumerate() {
            if *element == *c {
                elements.push((outer_i, inner_i));
            }
        }
    }
    if elements.len() < 1 {
        return None;
    }
    return Some(elements);
}

enum Direction {
    North,
    East,
    South,
    West,
}

struct Guard {
    position: (usize, usize),
    previous_positions: Vec<(usize, usize)>,
    direction: Direction,
    grid: Vec<Vec<char>>,
    grid_rows: usize,
    grid_cols: usize,
}
impl Guard {
    pub fn new(grid: Vec<Vec<char>>) -> Self {
        //let mut start = Vec::new();
        //start.push(get_position(&grid, &'^').unwrap()[0]);

        Guard {
            position: get_position(&grid, &'^').unwrap()[0],
            previous_positions: Vec::new(),
            direction: Direction::North,
            grid: grid.to_vec(),
            grid_rows: (grid.len() - 1),
            grid_cols: (grid[0].len() - 1),
        }
    }
    pub fn patrol(&mut self) {
        let movement = match self.direction {
            Direction::North => (-1, 0),
            Direction::East => (0, 1),
            Direction::South => (1, 0),
            Direction::West => (0, -1),
        };
        let next_pos = match self.next_pos(movement) {
            Some(pos) => pos,
            None => {
                self.previous_positions.push(self.position);
                // stepped off the map
                return;
            }
        };
        if self.test_next(&next_pos) {
            if !self.previous_positions.iter().any(|x| *x == next_pos) {
                self.previous_positions.push(self.position.clone());
            }
            self.position = next_pos;
            //println!("{:?}", next_pos);
        } else {
            self.turn_right();
        }
        self.patrol();
    }
    fn turn_right(&mut self) {
        self.direction = match self.direction {
            Direction::North => Direction::East,
            Direction::East => Direction::South,
            Direction::South => Direction::West,
            Direction::West => Direction::North,
        }
    }

    fn add(a: isize, b: isize) -> usize {
        if a < 0 && b > 0 {
            return b as usize - a.abs() as usize;
        } else if a > 0 && b < 0 {
            return a as usize - b.abs() as usize;
        } else if a < 0 && b < 0 {
            panic!("Condition not supported");
        } else {
            return (a + b) as usize;
        }
    }
    // Get the next position in the direction specified. If it is off the map the return None
    fn next_pos(&self, next: (isize, isize)) -> Option<(usize, usize)> {
        // guard walks off the map
        if (self.position.0 == 0 && next.0 == -1) || (self.position.1 == 0 && next.1 == -1) {
            return None;
        }
        let new_pos: (usize, usize) = (
            (Guard::add(self.position.0 as isize, next.0)),
            (Guard::add(self.position.1 as isize, next.1)),
        );
        if new_pos.0 > self.grid_rows || new_pos.1 > self.grid_cols {
            return None;
        }
        return Some(new_pos);
    }

    fn test_next(&self, next_pos: &(usize, usize)) -> bool {
        println!("{:?}", next_pos);
        if self.grid[next_pos.0][next_pos.1] == '#' {
            return false;
        }
        return true;
    }
}
fn main() {
    let file = fs::read_to_string("./src/input.txt").expect("File not found");
    //let file = fs::read_to_string("./src/test_input.txt").expect("File not found");

    let lines: Vec<_> = file.lines().collect();

    let grid: Vec<Vec<char>> = lines.iter().map(|l| l.chars().collect()).collect();

    let mut guard = Guard::new(grid);

    guard.patrol();

    //println!("Previous Positions: {:?}", guard.previous_positions);
    println!("{:?}", guard.previous_positions.len());
}
