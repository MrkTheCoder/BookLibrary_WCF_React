import React from "react";
import Footer from "../Footer";
import { render, fireEvent, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";

let getByTestId;
beforeEach(() => {
  const component = render(<Footer />);
  getByTestId = component.getByTestId;
});

test("footer renders with correct text", () => {
  const footerEl = getByTestId("Footer");
  expect(footerEl.textContent).toBe("Copyright © Library");
});
