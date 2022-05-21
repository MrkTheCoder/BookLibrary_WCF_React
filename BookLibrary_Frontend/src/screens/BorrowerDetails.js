import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { LinkContainer } from "react-router-bootstrap";
import { Image, Button, Table, Container } from "react-bootstrap";
import Loader from "../components/Loader";
import Message from "../components/Message";
import { BORROWER_DETAILS_RESET } from "../constants/borrowersConstants";
import "./BorrowerDetails.css";
import { borrowerDetails } from "../actions/borrowersActions";

function BorrowerDetails() {
  const dispatch = useDispatch();
  const borrowerFromState = useSelector((state) => state.borrower);
  const { borrower, loading, error } = borrowerFromState;
  const { email } = useParams("email");

  useEffect(() => {
    if (!borrower) {
      dispatch(borrowerDetails(email));
    }
    if (borrower) {
      if (borrower.Email != email) {
        dispatch({ type: BORROWER_DETAILS_RESET });
        dispatch(borrowerDetails(email));
      }
    }
  }, [dispatch]);

  const dateHandler = (date) => {
    const exp = /([0-9]+)\+([0-9]+)/;

    const [_, dateNumber, __] = exp.exec(date);
    const formattedDate = new Date(Number(dateNumber));

    return formattedDate.toString();
  };

  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">{error}</Message>
      ) : (
        <div>
          {borrower ? (
            <div className="detailsBody">
              <div>
                <img
                  className="card-image"
                  alt={borrower.Username}
                  src={borrower.ImageLink}
                />
              </div>
              <ul className="detailsText">
                <li className="detailsListItem">
                  <span className="DetailsListItemSpan">Name:</span>
                  {borrower.FirstName} {borrower.MiddleName} {borrower.LastName}
                </li>

                <li className="detailsListItem">
                  <span className="DetailsListItemSpan">Username:</span>
                  {borrower.Username}
                </li>
                <li className="detailsListItem">
                  <span className="DetailsListItemSpan">Phone number:</span>
                  {borrower.PhoneNo}
                </li>
                <li className="detailsListItem">
                  <span className="DetailsListItemSpan">Total borrows:</span>{" "}
                  {borrower.TotalBorrows}
                </li>
                <li className="detailsListItem">
                  <span className="DetailsListItemSpan">Gender:</span>{" "}
                  {borrower.Gender}
                </li>
                <li className="detailsListItem">
                  <span className="DetailsListItemSpan">
                    Registration date:
                  </span>
                  {dateHandler(borrower.RegistrationDate)}
                </li>
              </ul>
            </div>
          ) : (
            <></>
          )}
        </div>
      )}
    </div>
  );
}

export default BorrowerDetails;
