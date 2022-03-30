import React from "react";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";
import "./style.css";

export default function Bookcard(props) {
  const book = props.book;
  return props.book ? (
    <div className="bookCards">
      <Card data-testid="card" className=" my-3">
        <div className="cardBody">
          <Card.Body className="bookCards">
            <Card.Title className="cardTitle" data-testid="title">
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
          </Card.Body>
        </div>
        <span className="cardImage">
          <img data-testid="image" src={book.CoverLink} alt={book.Isbn} />
        </span>
      </Card>
    </div>
  ) : (
    <div data-testid="noData">Error 500</div>
  );
}
