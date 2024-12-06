use std::{fs::File, io::BufRead, io::BufReader};

fn main() {
    //let file_name: &str = "./src/test_input.txt";
    let file_name: &str = "./src/puzzle.txt";

    let file = File::open(&file_name).expect("File not found");

    let lines = BufReader::new(file).lines().flatten();

    let mut input: Vec<Vec<char>> = Vec::new();

    for row in lines.into_iter() {
        input.push(row.chars().collect());
    }

    //println!("{:?}", input);

    let mut puzzle = Puzzle::new(&input);

    process(&mut puzzle, input);

    println!("Found {} words", &puzzle.count);
}

struct Puzzle {
    count: usize,
    width: usize,
    height: usize,
}

impl Puzzle {
    pub fn new(input: &Vec<Vec<char>>) -> Self {
        Puzzle {
            count: 0,
            height: input.len() - 1,
            width: input[0].len() - 1,
        }
    }
}
#[derive(Debug)]
struct Buffer {
    x: usize,
    y: usize,
    max_width: usize,
    max_height: usize,
}
impl Buffer {
    pub fn new_from_cursor(cursor: &Cursor) -> Self {
        Buffer {
            x: cursor.x.clone(),
            y: cursor.y.clone(),
            max_height: cursor.max_width.clone(),
            max_width: cursor.max_width.clone(),
        }
    }
    pub fn new_from_buffer(buffer: &Buffer) -> Self {
        Buffer {
            x: buffer.x.clone(),
            y: buffer.y.clone(),
            max_height: buffer.max_width.clone(),
            max_width: buffer.max_width.clone(),
        }
    }
    pub fn next_y(&self) -> Option<usize> {
        let next_y = self.y + 1;

        if next_y > self.max_height {
            return None;
        }
        return Some(next_y);
    }
    pub fn next_x(&self) -> Option<usize> {
        let next_x = self.x + 1;

        if next_x > self.max_width {
            return None;
        }
        return Some(next_x);
    }
    pub fn previous_y(&self) -> Option<usize> {
        if self.y == 0 {
            return None;
        }
        let prev_y = self.y - 1;

        return Some(prev_y);
    }
    pub fn previous_x(&self) -> Option<usize> {
        if self.x == 0 {
            return None;
        }
        let prev_x = self.x - 1;

        return Some(prev_x);
    }
    // function to get the next buffer position to look for
    pub fn get_next(&mut self, direction: &Direction) -> Option<&Buffer> {
        match direction {
            Direction::Left => {
                let x = self.previous_x();
                match x {
                    None => None,
                    Some(next) => {
                        self.x = next;
                        Some(self)
                    }
                }
            }
            Direction::UpLeft => {
                let y = self.previous_y();
                let x = self.previous_x();
                if y.is_none() {
                    return None;
                }
                if x.is_none() {
                    return None;
                }
                self.x = x?;
                self.y = y?;

                Some(self)
            }
            Direction::Up => {
                let y = self.previous_y();
                match y {
                    None => None,
                    Some(next) => {
                        self.y = next;
                        return Some(self);
                    }
                }
            }
            Direction::UpRight => {
                let y = self.previous_y();
                let x = self.next_x();
                if y.is_none() {
                    return None;
                }
                if x.is_none() {
                    return None;
                }

                self.x = x?;
                self.y = y?;

                Some(self)
            }
            Direction::Right => {
                let x = self.next_x();

                match x {
                    None => None,
                    Some(next) => {
                        self.x = next;
                        Some(self)
                    }
                }
            }
            Direction::DownRight => {
                let y = self.next_y();
                let x = self.next_x();
                if y.is_none() {
                    return None;
                }
                if x.is_none() {
                    return None;
                }

                self.x = x?;
                self.y = y?;

                Some(self)
            }
            Direction::Down => {
                let y = self.next_y();
                match y {
                    None => None,
                    Some(next) => {
                        self.y = next;
                        return Some(self);
                    }
                }
            }
            Direction::DownLeft => {
                let y = self.next_y();
                let x = self.previous_x();

                if y.is_none() {
                    return None;
                }
                if x.is_none() {
                    return None;
                }

                self.x = x?;
                self.y = y?;

                return Some(self);
            }
        }
    }
}
#[derive(Debug)]
struct Cursor {
    x: usize,
    y: usize,
    max_height: usize,
    max_width: usize,
    has_more: bool,
}
#[derive(Debug)]
enum Direction {
    Left,
    UpLeft,
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
}
impl Cursor {
    pub fn new(puzzle: &Puzzle) -> Self {
        Cursor {
            x: 0,
            y: 0,
            max_height: puzzle.height.clone(),
            max_width: puzzle.width.clone(),
            has_more: true,
        }
    }
    pub fn next(&mut self) {
        if self.y == self.max_height && self.x == self.max_width {
            self.has_more = false;
            return;
        }
        let next_x = self.x + 1;

        if next_x > self.max_width {
            let next_y = self.y + 1;

            self.y = next_y;
            self.x = 0;
            return;
        }
        self.x = next_x;
    }
}

fn process(puzzle: &mut Puzzle, input: Vec<Vec<char>>) {
    let mut cursor: Cursor = Cursor::new(&puzzle);

    let word: Vec<char> = "XMAS".chars().collect();

    let mut directions: Vec<Direction> = Vec::new();
    directions.push(Direction::UpLeft);
    directions.push(Direction::Up);
    directions.push(Direction::UpRight);
    directions.push(Direction::Right);
    directions.push(Direction::DownRight);
    directions.push(Direction::Down);
    directions.push(Direction::DownLeft);
    directions.push(Direction::Left);

    while cursor.has_more {
        //println!("cursor {}, {}", cursor.x, cursor.y);
        if cursor.x == 0 {
            //println!(" : {:?}", input[cursor.y]);
        }
        if input[cursor.y][cursor.x] != word[0] {
            cursor.next();
            continue;
        }
        //println!("Found one: {}, {:?}", input[cursor.y][cursor.x], cursor);

        for direction in &directions {
            let buf = Buffer::new_from_cursor(&cursor);

            let result = search(&input, &buf, &direction, &word, 1);

            if result.is_some() {
                puzzle.count = puzzle.count + result.unwrap();
            }
        }

        cursor.next();
    }
}

fn search(
    input: &Vec<Vec<char>>,
    buffer: &Buffer,
    direction: &Direction,
    word: &Vec<char>,
    position: usize,
) -> Option<usize> {
    let mut buf = Buffer::new_from_buffer(&buffer);
    let buf = buf.get_next(direction);

    //println!(" - the direction: {:?} -  {:?}", direction, buf);
    if buf.is_none() {
        return None;
    }
    let buf = buf.unwrap();

    /*println!(
        "   - Value is: {} expect {}",
        input[buf.y][buf.x],
        word[position]
    );*/

    if input[buf.y][buf.x] == word[position] && position == (word.len() - 1) {
        //println!("***** FOUND ONE****");
        return Some(1);
    }
    if input[buf.y][buf.x] != word[position] {
        return None;
    }
    let p: usize = position + 1;

    return search(&input, buf, &direction, &word, p);
}
