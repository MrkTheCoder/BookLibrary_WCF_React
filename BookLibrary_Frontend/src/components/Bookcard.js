import React from "react";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";
import "./style.css";

export default function Bookcard(props) {
  return props.book ? (
    <Card data-testid="card" className="bookCards my-3">
      <Card.Body className="bookCards">
        <Card.Title className="cardTitle" data-testid="title">
          {props.book.Title}
        </Card.Title>

        <Card.Subtitle className="subTitle" data-testid="subTitle">
          {props.book.Isbn}
        </Card.Subtitle>
        <Card.Subtitle className="subTitle" data-testid="status">
          Status:{" "}
          <span
            className={
              props.book.IsAvailable == true
                ? "green"
                : props.book.IsAvailable == false
                ? "red"
                : "yellow"
            }
          >
            {props.book.IsAvailable == true
              ? "Available"
              : props.book.IsAvailable == false
              ? "Not available"
              : "unkown"}
          </span>
        </Card.Subtitle>
        <Card.Link>
          <Link data-testid="link" to={`/book/${props.book.Id}`}>
            More Details
          </Link>
        </Card.Link>
      </Card.Body>
    </Card>
  ) : (
    <div data-testid="noData">Error 500</div>
  );
}
