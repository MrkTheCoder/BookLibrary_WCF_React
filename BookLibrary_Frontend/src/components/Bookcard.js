import React from "react";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function Bookcard(props) {
  return props.book ? (
    <Card data-testid="card" className="bookCards my-3">
      <Card.Body className="bookCards">
        <Card.Title data-testid="title">{props.book.name}</Card.Title>
        <Card.Subtitle data-testid="subTitle">{props.book.id}</Card.Subtitle>
        <Card.Link>
          <Link to={`/book/${props.book.id}`}>More Details</Link>
        </Card.Link>
      </Card.Body>
    </Card>
  ) : (
    <div data-testid="noData">Error 500</div>
  );
}
