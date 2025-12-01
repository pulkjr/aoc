use std::fs;

fn main() {
    //let file = fs::read_to_string("./src/input.txt").expect("File not found");
    let file: String = fs::read_to_string("./src/test_input.txt").expect("File not found");

    let mut disk: Vec<u8> = Vec::new();

    for i in 0..(file.len() - 1) {
        println!("{:?}", file.index(i));
    }
}
