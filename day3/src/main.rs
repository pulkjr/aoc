use regex::Regex;
use std::fs;

fn main() {
    let memory = fs::read_to_string("./src/memory.txt").expect("Unable to read file");

    let re = Regex::new(r"mul\(([0-9]{1,3}),([0-9]{1,3})\)").unwrap();

    let mut sum: u32 = 0;

    for (_, [x, y]) in re.captures_iter(&memory).map(|c| c.extract()) {
        let x: u32 = x.parse().expect("Failed to parse string to integer");
        let y: u32 = y.parse().expect("Failed to parse string to integer");
        sum = sum + (y * x);
    }
    println!("{:?}", sum);
}
