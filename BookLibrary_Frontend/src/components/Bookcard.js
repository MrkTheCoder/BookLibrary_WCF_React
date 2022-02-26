import React from "react";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function Bookcard({ book }) {
  return (
    <Card className="bookCards my-3">
      <Card.Body className="bookCards">
        <Card.Title>{book.name}</Card.Title>
        <Card.Subtitle>{book.id}</Card.Subtitle>
        <Card.Link>
          <Link to={`/book/${book.id}`}>More Details</Link>
        </Card.Link>
      </Card.Body>
    </Card>
  );
}
