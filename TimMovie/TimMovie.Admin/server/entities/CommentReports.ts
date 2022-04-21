import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { Comments } from "./Comments";
import { AspNetUsers } from "./AspNetUsers";

@Index("IX_CommentReports_CommentId", ["commentId"], {})
@Index("PK_CommentReports", ["id"], { unique: true })
@Index("IX_CommentReports_UserId", ["userId"], {})
@Entity("CommentReports", { schema: "public" })
export class CommentReports {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "CommentId" })
  commentId: string;

  @Column("uuid", { name: "UserId" })
  userId: string;

  @ManyToOne(() => Comments, (comments) => comments.commentReports, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "CommentId", referencedColumnName: "id" }])
  comment: Comments;

  @ManyToOne(() => AspNetUsers, (aspNetUsers) => aspNetUsers.commentReports, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "UserId", referencedColumnName: "id" }])
  user: AspNetUsers;
}
