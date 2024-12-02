/*
Zechariah 9:9
Rejoice greatly, Daughter Zion!
    Shout, Daughter Jerusalem!
See, your king comes to you,
    righteous and victorious,
lowly and riding on a donkey,
    on a colt, the foal of a donkey.
*/
use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

fn find_count(right_list: &Vec<i32>, number: i32) -> usize {
    return right_list.iter().filter(|&x| *x == number).count();
}

fn main() {
    let lines = read_lines("./src/locationid.txt").unwrap();
    //let lines = read_lines("./src/day1_test.txt").unwrap();

    let mut answer1 = 0;

    let mut left: Vec<i32> = Vec::new();
    let mut right: Vec<i32> = Vec::new();

    for line in lines.flatten() {
        let location: Vec<&str> = line.split_whitespace().collect();

        left.push(location[0].parse::<i32>().unwrap());
        right.push(location[1].parse::<i32>().unwrap());
    }

    left.sort();
    right.sort();

    for i in 0..left.len() {
        answer1 = answer1 + (left[i] - right[i]).abs();
    }

    println!("Problem 1 Answer: {}", answer1);

    let mut answer2 = 0;
    for left_num in left.iter() {
        let count = find_count(&right, *left_num) as i32;
        answer2 = answer2 + (*left_num * count);
    }
    println!("Problem 2 Answer: {}", answer2);
}

// The output is wrapped in a Result to allow matching on errors.
// Returns an Iterator to the Reader of the lines of the file.
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where
    P: AsRef<Path>,
{
    let file = File::open(filename)?;

    Ok(io::BufReader::new(file).lines())
}
