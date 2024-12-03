use std::{fs::File, io::BufRead, io::BufReader};

#[derive(Debug)]
enum Order {
    Ascending,
    Descending,
}

fn determine_order(numbers: &[i32]) -> Option<Order> {
    if numbers.is_empty() {
        return None;
    }

    let mut asc = false;
    let mut desc = false;

    for w in numbers.windows(2) {
        if let [a, b] = w {
            println!("{} - {}", a, b);
            if (a - b).abs() > 3 {
                return None;
            } else if a == b {
                return None;
            } else if a > b && asc == true {
                return None;
            } else if a < b && desc == true {
                return None;
            } else if a > b {
                desc = true;
            } else {
                asc = true;
            }
        }
    }

    if asc {
        return Some(Order::Ascending);
    } else if desc {
        return Some(Order::Descending);
    } else {
        return None;
    }
}

fn main() {
    let file = File::open("./src/day2_test.txt").unwrap();
    //let file = File::open("./src/input.txt").unwrap();

    let lines = BufReader::new(file).lines().flatten();

    let mut safe_count: u32 = 0;

    for line in lines {
        let numbers: Vec<i32> = line
            .split_whitespace()
            .map(|s| s.parse().unwrap())
            .collect();

        match determine_order(&numbers) {
            Some(Order::Ascending) => {
                println!("SAFE ASC {:?}", &numbers);
                safe_count = safe_count + 1;
            }
            Some(Order::Descending) => {
                println!("SAFE DES {:?}", &numbers);
                safe_count = safe_count + 1;
            }
            None => safe_count = safe_count,
        }
    }
    println!("Final count is {}", safe_count);
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn expected_result() {}
}
