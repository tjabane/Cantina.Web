from pydantic import BaseModel

class Review(BaseModel):
    Id: int | None = None
    Comment: str | None = None