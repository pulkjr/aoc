use std::{fs::File, io::BufRead, io::BufReader};

struct Tree {
    root: Option<Box<Node>>,
    report: String,
}
impl Tree {
    pub fn new() -> Self {
        Tree {
            root: None,
            report: String::from("Safe"),
        }
    }
    pub fn insert(&mut self, value: i32) {
        match &mut self.root {
            None => {
                self.root = Node::new(value).into();
            }
            Some(node) => {
                Tree::insert_recursively(node, value);
            }
        }
    }
    pub fn insert_recursively(node: &mut Box<Node>, value: i32) -> Option<String> {
        if value > node.value {
            match &mut node.right {
                None => {
                    if node.left != None {
                        //TODO: Set the tree report to Unsafe
                        return Some(String::from("Unsafe"));
                    }
                    node.right = Node::new(value).into();
                    return None;
                }
                Some(n) => Tree::insert_recursively(n, value),
            }
        } else if value < node.value {
            match &mut node.left {
                None => {
                    // TODO: Deal with left branch
                    if node.right != None {
                        return Some(String::from("Unsafe"));
                    }
                    node.right = Node::new(value).into();
                    return None;
                }
                Some(n) => Tree::insert_recursively(n, value),
            }
        }
    }
}
impl From<Node> for Option<Box<Node>> {
    fn from(node: Node) -> Self {
        return Some(Box::new(node));
    }
}
struct Node {
    value: i32,
    left: Option<Box<Node>>,
    right: Option<Box<Node>>,
}
impl Node {
    pub fn new(value: i32) -> Self {
        Node {
            value,
            left: None,
            right: None,
        }
    }
}
fn main() {
    let file = File::open("./src/day2_test.txt").unwrap();

    let lines = BufReader::new(file).lines().flatten();

    for line in lines {
        let tree = Tree::new();

        for n in line.split_whitespace() {
            println!("{}", n);
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn expected_result() {
        let mut tree = Tree::new();

        tree.insert(9);
    }
}
