import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Card, Container, Col, ListGroup, Button } from "react-bootstrap";
import { listBorrowers } from "../actions/borrowersActions";

function Borrowers() {
  const dispatch = useDispatch();
  const borrowersFromState = useSelector((state) => state.borrowers);
  const { error, loading, borrowers } = borrowersFromState;

  useEffect(() => {
    borrowers.length == 0 && dispatch(listBorrowers());
    console.log(borrowers);
  }, [dispatch]);
  return <div>Borrowers</div>;
}

export default Borrowers;
