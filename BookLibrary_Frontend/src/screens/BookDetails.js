import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { Card, Container, Col, ListGroup, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { bookDetailsAction } from "../actions/bookActions";
import books from "../data";

function BookDetails() {
  const match = useParams();
  const dispatch = useDispatch();

  const { Isbn } = useParams("Isbn");
  console.log(Isbn);

  const bookDetails = useSelector((state) => state.bookDetails);
  const { loading, book, error } = bookDetails;

  useEffect(() => {
    if (!book || book.Isbn != Isbn) {
      dispatch(bookDetailsAction(Isbn));
    }
  }, [dispatch]);

  const addToListhandler = () => {
    console.log("book added");
  };
  return (
    <Container className="CustomContainer">
      {console.log(book)}
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
