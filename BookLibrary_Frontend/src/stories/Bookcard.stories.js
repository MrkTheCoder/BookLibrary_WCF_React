import React from "react";
import { MemoryRouter } from "react-router-dom";
import Bookcard from "../components/Bookcard";
import "../bootstrap.min.css";
import "../components/style.css";

export default {
  title: "Bookcard",
  component: Bookcard,
  decorators: [
    (Story) => (
      <MemoryRouter>
        <Story />
      </MemoryRouter>
    ),
  ],
};
const Template = (args) => <Bookcard {...args} />;

export const Default = Template.bind({});

Default.args = {
  book: {
    Id: 2,
    IsAvailable: true,
    Isbn: "101-22258489522",
    Title: "Learn Design Patterns in R",
  },
};
