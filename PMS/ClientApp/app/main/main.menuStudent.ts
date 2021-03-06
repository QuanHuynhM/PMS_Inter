export const PAGES_MENU_STUDENT = [
  {
    path: "main",
    children: [
      {
        path: "dashboard",
        data: {
          menu: {
            title: "general.menu.dashboard",
            icon: "fa fa-home",
            selected: false,
            expanded: false,
            order: 0
          }
        }
      },
      {
        path: "project",
        data: {
          menu: {
            title: "Project",
            icon: "fa fa-book",
            selected: false,
            expanded: false,
            order: 0
          }
        }
      },
      {
        path: "lecturer",
        data: {
          menu: {
            title: "Lecturer",
            icon: "fa fa-male",
            selected: false,
            expanded: false,
            order: 0
          }
        }
      },
      {
        path: "group",
        data: {
          menu: {
            title: "Group",
            icon: "fa fa-group",
            selected: false,
            expanded: false,
            order: 0
          }
        }
      },
      {
        path: "confirm-group",
        data: {
          menu: {
            title: "Confirm Group",
            icon: "fa fa-user-plus",
            selected: false,
            expanded: false,
            order: 0
          }
        }
      },
      {
        path: "tag",
        data: {
          menu: {
            title: "Tag",
            icon: "fa fa-tags",
            selected: false,
            expanded: false,
            order: 0
          }
        }
      }
    ]
  }
];
