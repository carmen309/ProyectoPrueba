// Connect button(s) to drawer(s)
var sidebarToggle = document.querySelectorAll('[data-toggle="sidebar"]')
sidebarToggle = Array.prototype.slice.call(sidebarToggle)

sidebarToggle.forEach(function (toggle) {
  toggle.addEventListener('click', function (e) {
    var selector = e.currentTarget.getAttribute('data-target') || '#default-drawer'
    var drawer = document.querySelector(selector)
    if (drawer) {
      drawer.mdkDrawer.toggle()
    }
  })
})

///////////////////////////////////
// Custom JavaScript can go here //
///////////////////////////////////
