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
            src={book.CoverLink}
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
