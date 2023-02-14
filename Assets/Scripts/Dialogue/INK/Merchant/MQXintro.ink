VAR selectQuest = -1

Hello, Pilgrim. I have heard from the Priestess of the great help you have brought to us.

If you can spare your time, I have a few tasks I would require your help for as well.

Of course, all will come with a generous reward.

May you be interested?
->quest_list

=== quest_list ===
+ I'm busy. -> goBack
+ Desert  -> quest1
+ Route -> quest2
+ Blood -> quest3

=== quest1 ===
~ selectQuest = -1
A
To the far Northwest of the Enchanted Forest, there is the Restless Wasteland.

We have been attempting to open a route towards there. Perhaps, further ahead, we will find more groups of survivors.

Unfortunately, the way there is already dangerous, and we have no idea what Beasts may roam that desert.

I will need you to travel there and gather information on the Beasts of the Wasteland.

+No
~ selectQuest = -1
I see. Would you be interested in lending help elsewhere?
->quest_list

+Yes
~ selectQuest = 2
Thank you, Pilgrim. I only need you to defeat a few and report back on them.

Be careful, for they are likely far more dangerous than those of the Enchanted Forest.

Good luck.

->END

=== quest2 ===
~ selectQuest = -1
A
Our foraging route through the Enchanted Forest is still infested with Beasts.

I'd like you to defeat a few Kraits, the weak Beasts near the Church, as well as some Vipers, stronger and fiercer, in the Northeast of the path.

Hopefully, with a bit of cleanup, the routes will become safer to use.

+No
~ selectQuest = -1
I see. How about something else, then?
->quest_list

+Yes
~ selectQuest = 3
Thank you, Pilgrim. This shouldn't be a problem for someone like you.

I appreciate the help. Be careful out there.

->END

=== quest3 ===
~ selectQuest = -1
A
You may have taken a few upgrades at our armory in the far right corner of the Church. They're quite handy, aren't they?

You see, Pilgrim, I forge those upgrades myself. But with our current conditions, I've been low on materials needed for them.

They consist of Beast parts, which aren't worth the risk. But for someone like you, these would be easy to collect.

So, what do you say? If you can gather these materials, I can forge more upgrades for your weapons.

+No
~ selectQuest = -1
Alright. Would you like to do something else instead?
->quest_list

+Yes
~ selectQuest = 4
Thank you. I need you to gather some Beast blood, as well as crystals which Beasts like to hoard.

These are crucial in the upgrades I've been forging for your abilities.

Good luck, and God bless you.

->END

== goBack ==
~ selectQuest = -1
A
Alright. You may return at any time if you're looking for work.

May God bless you.

->END
