import React from "react";
import { Card, Container, Col, ListGroup, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import books from "../data";

function BookDetails() {
  const addToListhandler = () => {
    console.log("book added");
  };
  return (
    <Container className="CustomContainer">
      <Col md={8}>
        <ListGroup>
          <ListGroup.Item>Price: $/day</ListGroup.Item>
          <ListGroup.Item>Available</ListGroup.Item>
          <ListGroup.Item>Available</ListGroup.Item>
        </ListGroup>
        <div className="navButtons">
          <LinkContainer to={"/"}>
            <Button className="bg-dark">Take me back </Button>
          </LinkContainer>

          <Button className="bg-dark" onClick={addToListhandler}>
            Add to list
          </Button>
        </div>
      </Col>
    </Container>
  );
}

export default BookDetails;
