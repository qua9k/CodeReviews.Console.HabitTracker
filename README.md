# Habit Tracker

A simple habit tracker. Occurrences of a habit are stored in a database via
CRUD operations. It uses `.NET`, `ADO.NET`, and `SQLite`.

## Purpose

Exposure to the above technologies.

## Design

The database is composed of a single table:

| id  | habit   | date       | count |
| --- | ------- | ---------- | ----- |
| 1   | Skiing  | 2026-01-01 | 3     |
| 2   | Writing | 2026-01-02 | 4     |
| 3   | Cooking | 2026-01-03 | 1     |

The database is interacted with via Command Line CRUD operations.

### Disclaimer

To facilitate effective learning, I don't use LLMs while working on personal
projects. I'm not a hater (professionally I'll adapt to whatever workflow is
expected), but I believe that LLM usage incurs a debt that can only be paid
with experiential learning. Don't take my word for it:

> "We find that AI use impairs conceptual understanding, code reading, and
> debugging abilities, without delivering significant efficiency gains on
> average. Participants who fully delegated coding tasks showed some
> productivity improvements, but at the cost of learning the library.
>
> Our findings suggest that AI-enhanced productivity is not a shortcut to
> competence and AI assistance should be carefully adopted into workflows to
> preserve skill formation."
>
> - Anthropic, [How AI Impacts Skill Formation](https://arxiv.org/abs/2601.20245)
