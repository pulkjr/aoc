use std::fs;

use itertools::Itertools;

fn main() {
    //let data = fs::read_to_string("./src/test_input.txt").unwrap();
    let data = fs::read_to_string("./src/input.txt").unwrap();

    let lines: Vec<_> = data.lines().collect();

    // Split the Vec into two on the blank line.
    let split: Vec<_> = lines.split(|l| l.is_empty()).collect();

    //The rules are the first Vec inside the split
    let rules: Vec<_> = split[0]
        .iter()
        .map(|line| {
            let mut split = line.split("|");
            (
                split.next().unwrap().parse::<usize>().unwrap(),
                split.next().unwrap().parse::<usize>().unwrap(),
            )
        })
        .collect();

    let updates: Vec<Vec<_>> = split[1]
        .iter()
        .map(|line| {
            line.split(",")
                .map(|n| n.parse::<usize>().unwrap())
                .collect()
        })
        .collect();

    let sum: usize = updates
        .iter()
        .filter(|update| {
            !update
                .iter()
                .combinations(2)
                //.map(|x| (x[0], x[1])) // This step is unnecessary as it is an array already.
                .any(|x| rules.iter().any(|r| r.1 == *x[0] && r.0 == *x[1]))
            // Looking for any number where the second value is to the left of the
            // pipe and the first value is to the right of the pipe.
        })
        .map(|u| u[u.len() / 2])
        .sum();

    println!("Final sum is: {}", sum);
}
