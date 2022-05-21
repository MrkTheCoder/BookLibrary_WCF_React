import React from "react";
import { Link } from "react-router-dom";
import "./Bookcardsv2.css";
import { Card, Button } from "react-bootstrap";

function Bookcardsv2(props) {
  const book = props.book;
  return (
    <div className="bookcard-container">
      <h6 className="bookcards-title">
        <p>{book.Title}</p>
      </h6>
      <div className="bookcard-img-info-container">
        <div className="img-container ">
          <img
            className="boockcards-img"
            src="data:image/png;base64,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"
            alt={book.Isbn}
          />
        </div>
        <ul className="bookcard-ul">
          <li className="bookcards-li">{book.Category}</li>
          <li className="bookcards-li">{book.Isbn}</li>
          <li>
            <Link className="bookcard-link" to={`/book/${book.Isbn}`}>
              <p>More Details</p>
            </Link>
          </li>
        </ul>
      </div>
    </div>
  );
}

export default Bookcardsv2;
