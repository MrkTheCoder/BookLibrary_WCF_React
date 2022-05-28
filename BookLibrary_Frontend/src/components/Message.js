import react from "react";
import { Alert, Col } from "react-bootstrap";

function Message({ variant, children }) {
  return (
    <Alert style={{ margin: "10px 0" }} variant={variant}>
      <Col md={11}>{children}</Col>
      <Col md={1}></Col>
    </Alert>
  );
}

export default Message;
