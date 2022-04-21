import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { Subscribes } from "./Subscribes";
import { AspNetUsers } from "./AspNetUsers";

@Index("PK_UserSubscribes", ["id"], { unique: true })
@Index("IX_UserSubscribes_SubscribeId", ["subscribeId"], {})
@Index("IX_UserSubscribes_SubscribedUserId", ["subscribedUserId"], {})
@Entity("UserSubscribes", { schema: "public" })
export class UserSubscribes {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "SubscribedUserId" })
  subscribedUserId: string;

  @Column("uuid", { name: "SubscribeId" })
  subscribeId: string;

  @Column("timestamp with time zone", { name: "StartDay" })
  startDay: Date;

  @Column("timestamp with time zone", { name: "EndDate" })
  endDate: Date;

  @ManyToOne(() => Subscribes, (subscribes) => subscribes.userSubscribes, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "SubscribeId", referencedColumnName: "id" }])
  subscribe: Subscribes;

  @ManyToOne(() => AspNetUsers, (aspNetUsers) => aspNetUsers.userSubscribes, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "SubscribedUserId", referencedColumnName: "id" }])
  subscribedUser: AspNetUsers;
}
