use regex::Regex;
use std::fs;

fn main() {
    let memory = fs::read_to_string("./src/test_input.txt").expect("Unable to read file");
    //let memory = fs::read_to_string("./src/input.txt").expect("Unable to read file");

    let rules_re = Regex::new(r"([0-9]{2})|([0-9]{2})").unwrap();
    let mut rules: Vec<(usize, usize)> = Vec::new();

    for (_, [x, y]) in rules_re.captures_iter(&memory).map(|c| c.extract()) {
        let tup: (usize, usize) = (
            x.parse().expect("Parse Error"),
            y.parse().expect("Parse Error"),
        );
        rules.push(tup);
    }

    let lines_re = Regex::new(r"[0-9]+,[0-9]+.+").unwrap();

    let mut sum: usize = 0;

    for found in lines_re.find_iter(&memory) {
        let line = found.as_str();
        let order: Vec<usize> = line
            .split(",")
            .map(|s| s.parse().expect("unable to parse"))
            .collect();

        for num in order {
            let num_re_str = format!(".*{}.*", num);
            let num_re = Regex::new(num_re_str.as_str()).expect("Issues");

            for tup in rules
                .clone()
                .into_iter()
                .filter(|t| t.0 == num || t.1 == num)
            {
                println!("{:?}", tup);
            }
        }
    }
    println!("{:?}", sum);
}
