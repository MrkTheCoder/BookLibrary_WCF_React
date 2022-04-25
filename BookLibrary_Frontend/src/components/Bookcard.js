import React from "react";
import { Card, Col } from "react-bootstrap";
import { Link } from "react-router-dom";
import "./style.css";

export default function Bookcard(props) {
  const book = props.book;
  return props.book ? (
    <div className="customCard">
      <Card data-testid="card" className="my-3 " id="cardFrame">
        <Card.Body className="bookCards">
          <div>
            <Card.Title id="cardTitle" data-testid="title">
              {book.Title}
            </Card.Title>

            <Card.Subtitle className="subTitle" data-testid="subTitle">
              {book.Isbn}
            </Card.Subtitle>
            <Card.Subtitle className="subTitle">
              Status:{" "}
              <span
                data-testid="status"
                className={
                  book.IsAvailable == true
                    ? "green"
                    : book.IsAvailable == false
                    ? "red"
                    : "yellow"
                }
              >
                {book.IsAvailable == true
                  ? "Available"
                  : book.IsAvailable == false
                  ? "Not available"
                  : "unkown"}
              </span>
            </Card.Subtitle>

            <Card.Link>
              <Link data-testid="link" to={`/book/${book.Isbn}`}>
                More Details
              </Link>
            </Card.Link>
          </div>
        </Card.Body>
        <div className="cardImg">
          <Card.Img
            variant="bottom"
            data-testid="image"
            src={book.CoverLink}
            alt={book.Isbn}
          />
        </div>
      </Card>
    </div>
  ) : (
    <div data-testid="noData">Error 500</div>
  );
}
