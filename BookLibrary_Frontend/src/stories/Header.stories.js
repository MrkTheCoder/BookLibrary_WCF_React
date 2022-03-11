import React from "react";
import { MemoryRouter } from "react-router-dom";
import Header from "../components/Header";
import "../bootstrap.min.css";

export default {
  title: "Header",
  component: Header,
  decorators: [
    (Story) => (
      <MemoryRouter>
        <Story />
      </MemoryRouter>
    ),
  ],
};

const Template = (args) => <Header />;

export const Default = {};
