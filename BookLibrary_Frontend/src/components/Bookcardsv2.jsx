import React from "react";
import { Link } from "react-router-dom";
import "./Bookcardsv2.css";
import { Card } from "react-bootstrap";

function Bookcardsv2(props) {
  const book = props.book;
  return (
    <div className="bookcards-body">
      <div className="img-containe">
        <img className="boockcards-img" src={book.CoverLink} alt={book.Isbn} />
      </div>

      <ul className="bookcards-ul">
        <li className="bookcards-li">
          <h6 className="bookcards-li-title">{book.Title}</h6>
        </li>
        <li className="bookcards-li">{book.Category}</li>
        <li className="bookcards-li"></li>
        <li className="bookcards-li">
          <Link to={`/book/${book.Isbn}`}>More Details</Link>
        </li>
      </ul>
    </div>
  );
}

export default Bookcardsv2;
