import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { Card, Container, Col, ListGroup, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { bookDetailsAction } from "../actions/bookActions";
import "./BookDetails.css";

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
      <section className="detailBody">
        <div className="detailsCard">
          <div className="detailsContent">
            <div className="detailsText">
              <div className="titleEl">
                <h2>{book.Title}</h2>
                <h6>{book.Isbn}</h6>
              </div>
              <div>
                <ul className="infoEl">
                  <li>Category: {book.Category}</li>
                  <li>
                    Status:{" "}
                    {book.IsAvailable == true ? (
                      <span>Available</span>
                    ) : (
                      <span>Not available</span>
                    )}
                  </li>
                </ul>
              </div>
            </div>
            <div className="detailsImage">
              <img src={book.CoverLink}></img>
            </div>
          </div>
        </div>
      </section>
    </Container>
  );
}

export default BookDetails;
